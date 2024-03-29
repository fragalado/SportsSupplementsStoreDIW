﻿using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCProyectoFinal.Controllers
{
    /// <summary>
    /// Controlador de la entidad Acceso
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccesoControlador : ControllerBase
    {
        public readonly Contexto context;

        public AccesoControlador(Contexto context)
        {
            this.context = context;
        }

        /// <summary>
        /// Método que devuelve todos los accesos de la base de datos
        /// </summary>
        /// <returns>Devuelve todos los accesos de la base de datos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Acceso>>> GetAccesos()
        {
            // Devolvemos toda la lista de accesos
            return context.Accesos.ToList();
        }

        /// <summary>
        /// Método que obtiene un acceso por el id pasado por parámetros
        /// </summary>
        /// <param name="id">Id del acceso en la base de datos</param>
        /// <returns>Devuelve un acceso</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Acceso>> GetAccesoById(long id)
        {
            // Obtenemos el acceso por el id
            var acceso = await context.Accesos.FindAsync(id);

            // Comprobamos si el acceso es nulo, si es nulo devolvemos not found
            if (acceso == null)
            {
                return NotFound();
            }

            // Si el acceso no es nulo se devolverá el acceso.
            return acceso;
        }

        /// <summary>
        /// Método que actualiza un acceso en la base de datos
        /// </summary>
        /// <param name="acceso">Acceso a actualizar</param>
        /// <returns>Devuelve el estado de la operacion</returns>
        [HttpPut]
        public async Task<IActionResult> PutAcceso(Acceso acceso)
        {
            // Cambiamos el estado del acceso a modificado, para que EntiyFramework realice la actualizacion
            context.Entry(acceso).State = EntityState.Modified;

            try
            {
                // Guardamos los cambios
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }

            return NoContent();
        }

        /// <summary>
        /// Método que crea un nuevo acceso en la base de datos
        /// </summary>
        /// <param name="acceso">Acceso a crear</param>
        /// <returns>Devuevel el acceso creado</returns>
        [HttpPost]
        public async Task<ActionResult<Acceso>> PostAcceso(Acceso acceso)
        {
            try
            {
                // Agregamos el acceso
                context.Accesos.Add(acceso);

                // Guardamos los cambios
                await context.SaveChangesAsync();

                // Devolvemos el acceso creado
                return CreatedAtAction("GetAccesoById", new { id = acceso.id_acceso }, acceso);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Método que elimina un acceso en la base de datos
        /// </summary>
        /// <param name="id">Id del acceso a eliminar</param>
        /// <returns>Devuelve el estado de la operacion</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAcceso(long id)
        {
            try
            {
                // Buscamos el acceso por su id
                var acceso = await context.Accesos.FindAsync(id);
                
                // Comprobamos si el acceso existe
                if (acceso == null)
                {
                    return NotFound();
                }

                // Eliminamos el acceso
                context.Accesos.Remove(acceso);

                // Guardamos los cambios
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
